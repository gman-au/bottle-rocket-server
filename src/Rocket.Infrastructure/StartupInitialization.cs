using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class StartupInitialization(
        ILogger<StartupInitialization> logger,
        IUserRepository userRepository,
        IEnumerable<IDatabasePreRunAction> databasePreRunActions,
        IPasswordGenerator passwordGenerator,
        IPasswordHasher passwordHasher
    ) : IStartupInitialization
    {
        public async Task InitializeAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                var preRunActions =
                    databasePreRunActions
                        .Select(o => o.RunAsync(cancellationToken));

                await
                    Task
                        .WhenAll(preRunActions);
            }
            catch (AggregateException ex)
            {
                foreach (var iex in ex.InnerExceptions)
                    logger
                        .LogError(
                            "A database pre-run action failed: {message}",
                            iex.Message
                        );

                throw;
            }

            logger
                .LogInformation("Checking first-start initialization...");

            var startupPhase =
                await
                    GetStartupPhaseAsync(cancellationToken);

            if (startupPhase != StartupPhaseEnum.NoUserAccounts)
            {
                logger
                    .LogInformation("Active admin account exists. Skipping first-start initialization.");

                return;
            }

            logger
                .LogWarning("FIRST START DETECTED - No active admin account found");

            logger
                .LogInformation("Creating initial admin account...");

            var password =
                passwordGenerator
                    .GeneratePassword();

            var passwordHash =
                passwordHasher
                    .HashPassword(password);

            var adminUser =
                new User
                {
                    Username = DomainConstants.RootAdminUserName,
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.UtcNow,
                    IsAdmin = true,
                    IsActive = true
                };

            await
                userRepository
                    .InsertUserAsync(
                        adminUser,
                        cancellationToken
                    );

            // Log the credentials prominently
            logger.LogWarning("================================================================================");
            logger.LogWarning("FIRST START - ADMIN ACCOUNT CREATED");
            logger.LogWarning("================================================================================");
            logger.LogWarning(
                "Username: {Username}",
                DomainConstants.RootAdminUserName
            );
            logger.LogWarning(
                "Password: {Password}",
                password
            );
            logger.LogWarning("================================================================================");
            logger.LogWarning("SAVE THESE CREDENTIALS - They will not be shown again!");
            logger.LogWarning("Use these credentials to log in and create your user account.");
            logger.LogWarning("The admin account will be deactivated after you create your first user.");
            logger.LogWarning("================================================================================");
        }

        public async Task<StartupPhaseEnum> GetStartupPhaseAsync(
            CancellationToken cancellationToken
        )
        {
            try
            {
                var adminUser =
                    await
                        userRepository
                            .GetUserByNameAsync(
                                DomainConstants.RootAdminUserName,
                                cancellationToken
                            );

                if (adminUser == null)
                    return
                        StartupPhaseEnum
                            .NoUserAccounts;

                if (adminUser.IsActive)
                    return
                        StartupPhaseEnum
                            .AdminPendingDeactivation;

                return
                    StartupPhaseEnum
                        .AdminDeactivated;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        ex,
                        "Error checking first-start status"
                    );

                throw;
            }
        }
    }
}