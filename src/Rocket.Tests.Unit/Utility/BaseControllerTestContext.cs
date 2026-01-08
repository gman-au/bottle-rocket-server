using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Rocket.Domain;
using Rocket.Domain.Utils;
using Rocket.Interfaces;
using Rocket.Tests.Unit.Extensions;

namespace Rocket.Tests.Unit.Utility
{
    public abstract class BaseControllerTestContext
    {
        private const string RootAdminUserId = "555555555555555555555555";
        private const string RootAdminUserName = "admin";

        protected const string AdminUserId = "ffffffffffffffffffffffff";
        private const string AdminUserName = "john.the.admin@test.com";

        private const string NonAdminUserId = "aaaaaaaaaaaaaaaaaaaaaaaa";
        private const string NonAdminUserName = "grant.the.normie@test.com";

        private const string InactiveAdminUserId = "999999999999999999999999";
        private const string InactiveAdminUserName = "roget.the.inactive@test.com";

        protected const string ValidUserIdToModify = "495df512ec7ac1581b3ee319";

        protected readonly IFixture Fixture;
        protected readonly IUserManager UserManager;

        protected ControllerContext RootAdminUserContext;
        protected ControllerContext AdminUserContext;
        protected ControllerContext InactiveAdminUserContext;
        protected ControllerContext NonAdminUserContext;

        protected BaseControllerTestContext()
        {
            Fixture =
                FixtureEx
                    .CreateNSubstituteFixture();

            UserManager = Fixture.Freeze<IUserManager>();
        }

        protected void SetupGetUserReturns()
        {
            var (
                rootAdminUser,
                adminActiveUser,
                nonAdminActiveUser,
                inactiveAdminActiveUser
                ) = SetupUsers();

            var someOtherUser =
                Fixture
                    .Create<User>();

            UserManager
                .GetUserByUserIdAsync(
                    RootAdminUserId,
                    CancellationToken.None
                )
                .Returns(rootAdminUser);

            UserManager
                .GetUserByUserIdAsync(
                    AdminUserId,
                    CancellationToken.None
                )
                .Returns(adminActiveUser);

            UserManager
                .GetUserByUserIdAsync(
                    InactiveAdminUserId,
                    CancellationToken.None
                )
                .Returns(inactiveAdminActiveUser);

            UserManager
                .GetUserByUserIdAsync(
                    NonAdminUserId,
                    CancellationToken.None
                )
                .Returns(nonAdminActiveUser);

            UserManager
                .GetUserByUserIdAsync(
                    ValidUserIdToModify,
                    CancellationToken.None
                )
                .Returns(someOtherUser);
        }

        protected void SetupCreateUserAccountReturns()
        {
            var someOtherUser =
                Fixture
                    .Create<User>();

            UserManager
                .CreateUserAccountAsync(
                    null,
                    null,
                    false,
                    CancellationToken.None
                )
                .ReturnsForAnyArgs(someOtherUser);
        }

        protected void SetupControllerContexts()
        {
            RootAdminUserContext =
                CreateControllerContext(
                    RootAdminUserId,
                    RootAdminUserName
                );
            
            AdminUserContext =
                CreateControllerContext(
                    AdminUserId,
                    AdminUserName
                );

            NonAdminUserContext =
                CreateControllerContext(
                    NonAdminUserId,
                    NonAdminUserName
                );

            InactiveAdminUserContext =
                CreateControllerContext(
                    InactiveAdminUserId,
                    InactiveAdminUserName
                );
        }

        private (User, User, User, User) SetupUsers()
        {
            var rootAdminUser =
                Fixture
                    .Build<User>()
                    .With(
                        o => o.Id,
                        RootAdminUserId
                    )
                    .With(
                        o => o.Username,
                        RootAdminUserName
                    )
                    .With(
                        o => o.IsActive,
                        true
                    )
                    .With(
                        o => o.IsAdmin,
                        true
                    )
                    .Create();
            
            var adminActiveUser =
                Fixture
                    .Build<User>()
                    .With(
                        o => o.Id,
                        AdminUserId
                    )
                    .With(
                        o => o.Username,
                        AdminUserName
                    )
                    .With(
                        o => o.IsActive,
                        true
                    )
                    .With(
                        o => o.IsAdmin,
                        true
                    )
                    .Create();

            var nonAdminActiveUser =
                Fixture
                    .Build<User>()
                    .With(
                        o => o.Id,
                        NonAdminUserId
                    )
                    .With(
                        o => o.Username,
                        NonAdminUserName
                    )
                    .With(
                        o => o.IsActive,
                        true
                    )
                    .With(
                        o => o.IsAdmin,
                        false
                    )
                    .Create();

            var inactiveAdminActiveUser =
                Fixture
                    .Build<User>()
                    .With(
                        o => o.Id,
                        InactiveAdminUserId
                    )
                    .With(
                        o => o.Username,
                        InactiveAdminUserName
                    )
                    .With(
                        o => o.IsActive,
                        false
                    )
                    .With(
                        o => o.IsAdmin,
                        true
                    )
                    .Create();

            return (
                rootAdminUser,
                adminActiveUser,
                nonAdminActiveUser,
                inactiveAdminActiveUser
            );
        }

        private static ControllerContext CreateControllerContext(
            string userId,
            string userName
        )
        {
            var claims = new List<Claim>
            {
                new(
                    ClaimTypes.Name,
                    userName
                ),
                new(
                    ClaimTypes.NameIdentifier,
                    userId
                )
            };

            var identity =
                new ClaimsIdentity(
                    claims,
                    DomainConstants.BasicAuthentication
                );

            return
                new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(identity)
                    }
                };
        }
    }
}