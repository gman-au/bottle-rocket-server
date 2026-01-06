using System;
using System.Net.Mail;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class EmailAddressValidator : IEmailAddressValidator
    {
        public bool IsValid(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                return false;

            try
            {
                var email = 
                    new MailAddress(emailAddress);

                return 
                    email
                        .Address == emailAddress; 
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            } 
        }
    }
}