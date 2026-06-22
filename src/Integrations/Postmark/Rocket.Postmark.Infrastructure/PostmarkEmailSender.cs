using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Postmark.Exceptions;
using PostmarkDotNet;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Postmark.Infrastructure.Extensions;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkEmailSender(
        ILogger<PostmarkEmailSender> logger
    ) : IPostmarkEmailSender
    {
        public async Task SendEmailAsync(
            string serverToken,
            byte[] attachmentData,
            string attachmentName,
            string attachmentContentType,
            string recipientAddress,
            string senderAddress,
            string subjectName,
            CancellationToken cancellationToken
        )
        {
            try
            {
                logger
                    .LogInformation("Sending email via Postmark");

                cancellationToken
                    .ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(serverToken))
                    throw new RocketException(
                        "No Postmark server token was provided. Please check the Postmark connector configuration.",
                        ApiStatusCodeEnum.ServerConfigurationError
                    );

                if (!senderAddress.IsValidEmail())
                    throw new RocketException(
                        "Sender address is not a valid email address.",
                        ApiStatusCodeEnum.ValidationError
                    );

                if (!recipientAddress.IsValidEmail())
                    throw new RocketException(
                        "Recipient address is not a valid email address.",
                        ApiStatusCodeEnum.ValidationError
                    );
                
                var client =
                    new PostmarkClient(serverToken);

                var attachment =
                    new PostmarkMessageAttachment(
                        attachmentData,
                        attachmentName,
                        attachmentContentType
                    );

                var message =
                    new PostmarkMessage
                    {
                        From = senderAddress,
                        To = recipientAddress,
                        Attachments = new List<PostmarkMessageAttachment> { attachment },
                        Subject = subjectName,
                    };

                var sendResult =
                    await
                        client
                            .SendMessageAsync(message);
            }
            catch (PostmarkResponseException ex)
            {
                logger
                    .LogError(
                        "There was an error emailing the file via Postmark: {message}",
                        ex.Message
                    );

                throw new RocketException(
                    "There was an error emailing the file via Postmark.",
                    ApiStatusCodeEnum.ThirdPartyServiceError,
                    (int)HttpStatusCode.InternalServerError,
                    ex
                );
            }
        }
    }
}