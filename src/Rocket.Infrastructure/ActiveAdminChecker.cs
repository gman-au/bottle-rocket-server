using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ActiveAdminChecker(IUserRepository userRepository) : IActiveAdminChecker
    {
        public async Task<bool> PerformAsync(
            string proposedUserId,
            bool? proposedIsActive,
            bool? proposedIsAdmin,
            CancellationToken cancellationToken
        )
        {
            if (proposedIsActive == null && proposedIsAdmin == null) return true;

            var (records, recordCount) = await
                userRepository
                    .GetActiveAdminsAsync(cancellationToken);

            var adminUsers = records as User[] ?? records.ToArray();

            if (adminUsers.Any(o => o.Id == proposedUserId))
            {
                switch (recordCount)
                {
                    case 1:
                    {
                        var singleRecord =
                            adminUsers
                                .First();

                        var proposedAdminIsOnlyAdmin = singleRecord.Id == proposedUserId;

                        if (proposedAdminIsOnlyAdmin)
                        {
                            if (proposedIsActive == false) return false;
                            if (proposedIsAdmin == false) return false;
                        }

                        break;
                    }
                    case > 1:
                        return true;
                }
            }
            else
            {
                // check we aren't assigning admin status to an inactive non-admin
                var proposedUser =
                    await
                        userRepository
                            .GetUserByUserIdAsync(
                                proposedUserId,
                                cancellationToken
                            );

                if (proposedIsAdmin != true) return true;
                
                // if inactive and not setting to active -> false
                if (proposedUser?.IsActive == false && proposedIsActive != true) return false;
                    
                // if active and setting to inactive -> false
                if (proposedUser?.IsActive == true && proposedIsActive == false) return false;

                return true;
            }

            return false;
        }
    }
}