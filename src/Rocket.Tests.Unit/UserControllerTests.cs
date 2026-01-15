using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rocket.Api.Contracts.Users;
using Rocket.Api.Host.Controllers;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;
using Rocket.Tests.Unit.Utility;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class UserControllerTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public async Task Test_Phase_2_Get_Valid_User_As_Admin()
        {
            _context.ArrangeLoggedInAdminUser();
            _context.ArrangeValidUserReturnedFromSearch();
            await _context.ActGetUserAsync();
            _context.AssertOkResult();
        }

        [Fact]
        public async Task Test_Phase_2_Get_No_User_As_Admin()
        {
            _context.ArrangeLoggedInAdminUser();
            _context.ArrangeNoUserReturnedFromSearch();
            await _context.ActGetUserWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.UnknownUser);
        }

        [Fact]
        public async Task Test_Phase_2_Get_Valid_User_As_Non_Admin()
        {
            _context.ArrangeLoggedInNonAdminUser();
            _context.ArrangeValidUserReturnedFromSearch();
            await _context.ActGetUserWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.RequiresAdministratorAccess);
        }

        [Fact]
        public async Task Test_Phase_2_Get_Valid_User_As_Inactive_Admin()
        {
            _context.ArrangeLoggedInInactiveAdminUser();
            _context.ArrangeValidUserReturnedFromSearch();
            await _context.ActGetUserWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.InactiveUser);
        }

        [Fact]
        public async Task Test_Phase_2_Get_No_User_As_Non_Admin()
        {
            _context.ArrangeLoggedInNonAdminUser();
            _context.ArrangeNoUserReturnedFromSearch();
            await _context.ActGetUserWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.RequiresAdministratorAccess);
        }

        [Fact]
        public async Task Test_Phase_1_Create_Non_New_Admin_User_As_Root_Admin()
        {
            _context.ArrangeStartupPhaseAdminPendingDeactivation();
            _context.ArrangeLoggedInRootAdminUser();
            _context.ArrangeValidCreateUserAsNotNewAdminRequest();
            await _context.ActCreateUserAsync();
            _context.AssertOkResult();
            _context.AssertCreateAccountIsAdminWasCalledWithTrue();
            _context.AssertDeactivationAdminWasCalled();
            _context.AssertUpdateAccountIsAdminWasNotCalled();
        }

        [Fact]
        public async Task Test_Phase_1_Create_Non_New_Admin_User_As_Admin()
        {
            _context.ArrangeStartupPhaseAdminDeactivated();
            _context.ArrangeLoggedInAdminUser();
            _context.ArrangeValidCreateUserAsNotNewAdminRequest();
            await _context.ActCreateUserAsync();
            _context.AssertOkResult();
            _context.AssertCreateAccountIsAdminWasCalledWithFalse();
            _context.AssertDeactivationAdminWasNotCalled();
            _context.AssertUpdateAccountIsAdminWasNotCalled();
        }

        [Fact]
        public async Task Test_Phase_1_Create_New_Admin_User_As_Admin()
        {
            _context.ArrangeStartupPhaseAdminPendingDeactivation();
            _context.ArrangeLoggedInAdminUser();
            _context.ArrangeValidCreateUserAsNewAdminRequest();
            await _context.ActCreateUserAsync();
            _context.AssertOkResult();
            _context.AssertCreateAccountIsAdminWasCalledWithTrue();
            _context.AssertDeactivationAdminWasNotCalled();
            _context.AssertUpdateAccountIsAdminWasCalledWithFalse();
        }

        [Fact]
        public async Task Test_Phase_1_Create_New_Admin_User_As_Inactive_Admin()
        {
            _context.ArrangeStartupPhaseAdminPendingDeactivation();
            _context.ArrangeLoggedInInactiveAdminUser();
            _context.ArrangeNoUserReturnedFromSearch();
            await _context.ActGetUserWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.InactiveUser);
        }

        [Fact]
        public async Task Test_Phase_1_Create_New_Admin_User_As_Non_Admin()
        {
            _context.ArrangeStartupPhaseAdminPendingDeactivation();
            _context.ArrangeLoggedInNonAdminUser();
            _context.ArrangeNoUserReturnedFromSearch();
            await _context.ActGetUserWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.RequiresAdministratorAccess);
        }

        [Fact]
        public async Task Test_Phase_1_Update_Non_New_Admin_User_As_Admin()
        {
            _context.ArrangeApprovedByAdminChecker();
            _context.ArrangeStartupPhaseAdminDeactivated();
            _context.ArrangeLoggedInAdminUser();
            _context.ArrangeValidMinimalUpdateUserAsNotNewAdminRequest();
            await _context.ActUpdateUserAsync();
            _context.AssertOkResult();
            _context.AssertUpdateAccountIsAdminWasNotCalled();
            _context.AssertUpdateAccountWasCalledWithMinimalFalse();
        }

        [Fact]
        public async Task Test_Phase_1_Update_Non_New_Admin_User_As_Non_Admin()
        {
            _context.ArrangeApprovedByAdminChecker();
            _context.ArrangeStartupPhaseAdminDeactivated();
            _context.ArrangeLoggedInNonAdminUser();
            _context.ArrangeValidMinimalUpdateUserAsNotNewAdminRequest();
            await _context.ActUpdateUserWithExceptionAsync();
            _context.AssertUpdateAccountIsAdminWasNotCalled();
            _context.AssertUpdateAccountWasNotCalled();
            _context.AssertExceptionCode(ApiStatusCodeEnum.RequiresAdministratorAccess);
        }

        [Fact]
        public async Task Test_Phase_1_Update_Non_New_Admin_User_As_Inactive_Admin()
        {
            _context.ArrangeApprovedByAdminChecker();
            _context.ArrangeStartupPhaseAdminDeactivated();
            _context.ArrangeLoggedInInactiveAdminUser();
            _context.ArrangeValidMinimalUpdateUserAsNotNewAdminRequest();
            await _context.ActUpdateUserWithExceptionAsync();
            _context.AssertUpdateAccountIsAdminWasNotCalled();
            _context.AssertUpdateAccountWasNotCalled();
            _context.AssertExceptionCode(ApiStatusCodeEnum.InactiveUser);
        }

        [Fact]
        public async Task Test_Phase_1_Update_New_Admin_User_As_Admin()
        {
            _context.ArrangeApprovedByAdminChecker();
            _context.ArrangeStartupPhaseAdminDeactivated();
            _context.ArrangeLoggedInAdminUser();
            _context.ArrangeValidUpdateUserAsNewAdminRequest();
            await _context.ActUpdateUserAsync();
            _context.AssertOkResult();
            _context.AssertUpdateAccountIsAdminWasCalledWithFalse();
            _context.AssertUpdateAccountWasCalledWithAdminTrue();
        }

        [Fact]
        public async Task Test_Phase_1_Update_New_Admin_User_As_Admin_Not_Approved()
        {
            _context.ArrangeNotApprovedByAdminChecker();
            _context.ArrangeStartupPhaseAdminDeactivated();
            _context.ArrangeLoggedInAdminUser();
            _context.ArrangeValidUpdateUserAsNewAdminRequest();
            await _context.ActUpdateUserWithExceptionAsync();
            _context.AssertUpdateAccountIsAdminWasNotCalled();
            _context.AssertUpdateAccountWasNotCalled();
            _context.AssertExceptionCode(ApiStatusCodeEnum.PotentiallyIrrecoverableOperation);
        }

        private class TestContext : BaseControllerTestContext<UserController>
        {
            private readonly IStartupInitialization _startupInitialization;
            private readonly IActiveAdminChecker _activeAdminChecker;
            private RocketException _exception;
            private IActionResult _result;
            private CreateUserRequest _userToCreate;
            private string _userIdToSearch;
            private UserDetail _userToUpdate;

            public TestContext()
            {
                _startupInitialization = Fixture.Freeze<IStartupInitialization>();
                _activeAdminChecker = Fixture.Freeze<IActiveAdminChecker>();

                Sut =
                    new UserController(
                        Fixture.Freeze<ILogger<UserController>>(),
                        UserManager,
                        Fixture.Freeze<IUserRepository>(),
                        _startupInitialization,
                        _activeAdminChecker
                    );

                SetupGetUserReturns();
                SetupCreateUserAccountReturns();
                SetupControllerContexts();
            }

            public void ArrangeApprovedByAdminChecker() =>
                _activeAdminChecker
                    .PerformAsync(
                        null,
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(true);

            public void ArrangeNotApprovedByAdminChecker() =>
                _activeAdminChecker
                    .PerformAsync(
                        null,
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(false);

            public void ArrangeStartupPhaseAdminDeactivated() =>
                _startupInitialization
                    .GetStartupPhaseAsync(CancellationToken.None)
                    .ReturnsForAnyArgs(StartupPhaseEnum.AdminDeactivated);

            public void ArrangeStartupPhaseAdminPendingDeactivation() =>
                _startupInitialization
                    .GetStartupPhaseAsync(CancellationToken.None)
                    .ReturnsForAnyArgs(StartupPhaseEnum.AdminPendingDeactivation);

            public void ArrangeValidUserReturnedFromSearch() => _userIdToSearch = ValidUserIdToModify;

            public void ArrangeNoUserReturnedFromSearch() => _userIdToSearch = "17592812e239877900dc092d";

            public void ArrangeValidMinimalUpdateUserAsNotNewAdminRequest() =>
                _userToUpdate = new UserDetail
                {
                    Id = ValidUserIdToModify,
                    Username = "user",
                    IsActive = null,
                    IsAdmin = null,
                    NewPassword = null
                };

            public void ArrangeValidUpdateUserAsNewAdminRequest() =>
                _userToUpdate = new UserDetail
                {
                    Id = ValidUserIdToModify,
                    Username = null,
                    IsActive = null,
                    IsAdmin = true,
                    NewPassword = null
                };

            public void ArrangeValidCreateUserAsNotNewAdminRequest() =>
                _userToCreate = new CreateUserRequest
                {
                    Username = "user",
                    Password = "password",
                    IsTheNewAdmin = false
                };

            public void ArrangeValidCreateUserAsNewAdminRequest() =>
                _userToCreate = new CreateUserRequest
                {
                    Username = "user",
                    Password = "password",
                    IsTheNewAdmin = true
                };

            public async Task ActGetUserAsync()
            {
                _result =
                    await
                        Sut
                            .GetUserAsync(
                                _userIdToSearch,
                                CancellationToken.None
                            );
            }

            public async Task ActUpdateUserAsync()
            {
                _result =
                    await
                        Sut
                            .UpdateUserAsync(
                                _userToUpdate,
                                CancellationToken.None
                            );
            }

            public async Task ActCreateUserAsync()
            {
                _result =
                    await
                        Sut
                            .CreateUserAsync(
                                _userToCreate,
                                CancellationToken.None
                            );
            }

            public async Task ActGetUserWithExceptionAsync() =>
                _exception =
                    await
                        Assert
                            .ThrowsAsync<RocketException>(ActGetUserAsync);

            public async Task ActUpdateUserWithExceptionAsync() =>
                _exception =
                    await
                        Assert
                            .ThrowsAsync<RocketException>(ActUpdateUserAsync);

            public void AssertOkResult()
            {
                Assert.NotNull(_result);
                var okResult = Assert.IsType<OkObjectResult>(_result);
                Assert.Equal(
                    (int)HttpStatusCode.OK,
                    okResult.StatusCode
                );
            }

            public void AssertExceptionCode(ApiStatusCodeEnum expected)
            {
                Assert.Null(_result);
                Assert.NotNull(_exception);
                Assert.Equal(
                    (int)expected,
                    _exception.ApiStatusCode
                );
            }

            public void AssertDeactivationAdminWasNotCalled() =>
                UserManager
                    .ReceivedWithAnyArgs(0)
                    .DeactivateAdminAccountAsync(CancellationToken.None);

            public void AssertDeactivationAdminWasCalled() =>
                UserManager
                    .ReceivedWithAnyArgs(1)
                    .DeactivateAdminAccountAsync(CancellationToken.None);

            public void AssertUpdateAccountIsAdminWasNotCalled() =>
                UserManager
                    .ReceivedWithAnyArgs(0)
                    .UpdateAccountIsAdminAsync(
                        null,
                        false,
                        CancellationToken.None
                    );

            public void AssertUpdateAccountIsAdminWasCalledWithFalse() =>
                UserManager
                    .Received(1)
                    .UpdateAccountIsAdminAsync(
                        AdminUserId,
                        false,
                        CancellationToken.None
                    );

            public void AssertCreateAccountIsAdminWasCalledWithTrue() =>
                UserManager
                    .Received(1)
                    .CreateUserAccountAsync(
                        "user",
                        "password",
                        true,
                        CancellationToken.None
                    );

            public void AssertCreateAccountIsAdminWasCalledWithFalse() =>
                UserManager
                    .Received(1)
                    .CreateUserAccountAsync(
                        "user",
                        "password",
                        false,
                        CancellationToken.None
                    );

            public void AssertUpdateAccountWasNotCalled() =>
                UserManager
                    .ReceivedWithAnyArgs(0)
                    .UpdateAccountAsync(
                        null,
                        null,
                        null,
                        null,
                        null,
                        CancellationToken.None
                    );

            public void AssertUpdateAccountWasCalled() =>
                UserManager
                    .ReceivedWithAnyArgs(1)
                    .UpdateAccountAsync(
                        null,
                        null,
                        true,
                        null,
                        null,
                        CancellationToken.None
                    );

            public void AssertUpdateAccountWasCalledWithMinimalFalse() =>
                UserManager
                    .Received(1)
                    .UpdateAccountAsync(
                        ValidUserIdToModify,
                        "user",
                        null,
                        null,
                        null,
                        CancellationToken.None
                    );

            public void AssertUpdateAccountWasCalledWithAdminTrue() =>
                UserManager
                    .Received(1)
                    .UpdateAccountAsync(
                        ValidUserIdToModify,
                        null,
                        null,
                        true,
                        null,
                        CancellationToken.None
                    );
        }
    }
}