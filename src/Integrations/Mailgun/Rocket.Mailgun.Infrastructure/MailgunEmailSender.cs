using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Mailgun;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;

namespace Rocket.Mailgun.Infrastructure
{
    public class MailgunEmailSender(ILogger<MailgunSender> logger) : IMailgunEmailSender
    {
        public async Task SendEmailAsync(
            string senderDomain,
            string apiKey,
            byte[] attachmentData,
            string attachmentName,
            string recipientAddress,
            string senderAddress,
            string subjectName,
            CancellationToken cancellationToken
        )
        {
            var sender =
                new MailgunSender(
                    senderDomain,
                    apiKey
                );

            Email.DefaultSender = sender;

            using var stream = new MemoryStream(attachmentData);

            var attachment =
                new Attachment
                {
                    Filename = attachmentName,
                    Data = stream,
                    ContentType = "application/octet-stream"
                };

            try
            {
                var email =
                    await
                        Email
                            .From(senderAddress)
                            .To(recipientAddress)
                            .Body("Please find the file attached.")
                            .Subject(subjectName)
                            .Attach(attachment)
                            .SendAsync(cancellationToken);

                if (!(email?.Successful).GetValueOrDefault())
                    throw new RocketException(
                        string.Join(
                            "\r\n",
                            email?.ErrorMessages ?? new[] { "Unknown error" }.ToList()
                        ),
                        ApiStatusCodeEnum.ThirdPartyServiceError
                    );
            }
            catch (Exception ex)
            {
                logger
                    .LogError("The Mailgun send step failed: {message}", ex.Message);
                
                throw;
            }
        }
    }
}