using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rocket.Api.Host.Controllers;
using Rocket.Domain;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Utils;
using Rocket.Interfaces;
using Rocket.Tests.Unit.Extensions;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class UserControllerTests
    {
        private const string AdminUserId = "ffffffffffffffffffffffff";
        private const string AdminUserName = "john.the.admin@test.com";     
        
        private const string NonAdminUserId = "aaaaaaaaaaaaaaaaaaaaaaaa";
        private const string NonAdminUserName = "grant.the.normie@test.com";
        
        private const string ValidUserIdToGet = "495df512ec7ac1581b3ee319";
        
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
        public async Task Test_Phase_2_Get_No_User_As_Non_Admin()
        {
            _context.ArrangeLoggedInNonAdminUser();
            _context.ArrangeNoUserReturnedFromSearch();
            await _context.ActGetUserWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.RequiresAdministratorAccess);
        }

        private class TestContext
        {
            private readonly UserController _sut;
            private readonly IFixture _fixture;
            private readonly IUserManager _userManager;
            private string _userIdToSearch;
            private IActionResult _result;
            private IStartupInitialization _startupInitialization;
            private ControllerContext _loggedInAdminUserContext;
            private ControllerContext _loggedInNonAdminUserContext;
            private RocketException _exception;

            public TestContext()
            {
                _fixture =
                    FixtureEx
                        .CreateNSubstituteFixture();

                _userManager = _fixture.Freeze<IUserManager>();
                _startupInitialization = _fixture.Freeze<IStartupInitialization>();

                _sut =
                    new UserController(
                        _fixture.Freeze<ILogger<UserController>>(),
                        _userManager,
                        _startupInitialization
                    );

                SetupUsersAndClaims();
                SetupAdminControllerContexts();
                SetupNonAdminControllerContexts();
            }

            private void SetupUsersAndClaims()
            {
                var adminActiveUser =
                    _fixture
                        .Build<User>()
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
                    _fixture
                        .Build<User>()
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
                            false
                        )
                        .Create();

                var someOtherUser =
                    _fixture
                        .Create<User>();

                _userManager
                    .GetUserByUserIdAsync(
                        AdminUserId,
                        CancellationToken.None
                    )
                    .Returns(adminActiveUser);

                _userManager
                    .GetUserByUserIdAsync(
                        NonAdminUserId,
                        CancellationToken.None
                    )
                    .Returns(nonAdminActiveUser);

                _userManager
                    .GetUserByUserIdAsync(
                        ValidUserIdToGet,
                        CancellationToken.None
                    )
                    .Returns(someOtherUser);
            }

            private void SetupAdminControllerContexts()
            {
                var claims = new List<Claim>
                {
                    new(
                        ClaimTypes.Name,
                        AdminUserName
                    ),
                    new(
                        ClaimTypes.NameIdentifier,
                        AdminUserId
                    )
                };

                var identity =
                    new ClaimsIdentity(
                        claims,
                        DomainConstants.BasicAuthentication
                    );

                _loggedInAdminUserContext =
                    new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            User = new ClaimsPrincipal(identity)
                        }
                    };
            }
            
            private void SetupNonAdminControllerContexts()
            {
                var claims = new List<Claim>
                {
                    new(
                        ClaimTypes.Name,
                        NonAdminUserName
                    ),
                    new(
                        ClaimTypes.NameIdentifier,
                        NonAdminUserId
                    )
                };

                var identity =
                    new ClaimsIdentity(
                        claims,
                        DomainConstants.BasicAuthentication
                    );

                _loggedInNonAdminUserContext =
                    new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            User = new ClaimsPrincipal(identity)
                        }
                    };
            }

            public void ArrangeLoggedInAdminUser()
            {
                _sut.ControllerContext = _loggedInAdminUserContext;
            }

            public void ArrangeLoggedInNonAdminUser()
            {
                _sut.ControllerContext = _loggedInNonAdminUserContext;
            }

            public void ArrangeValidUserReturnedFromSearch() => _userIdToSearch = ValidUserIdToGet;
            
            public void ArrangeNoUserReturnedFromSearch() => _userIdToSearch = "17592812e239877900dc092d";

            public async Task ActGetUserAsync()
            {
                _result =
                    await
                        _sut
                            .GetUserAsync(
                                _userIdToSearch,
                                CancellationToken.None
                            );
            }
            
            public async Task ActGetUserWithExceptionAsync() =>
                _exception = 
                    await
                        Assert
                            .ThrowsAsync<RocketException>(ActGetUserAsync);
            
            public void AssertOkResult()
            {
                Assert.NotNull(_result);
                var okResult = Assert.IsType<OkObjectResult>(_result);
                Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            }

            public void AssertExceptionCode(ApiStatusCodeEnum expected)
            {
                Assert.Null(_result);
                Assert.NotNull(_exception);
                Assert.Equal((int)expected, _exception.ApiStatusCode);
            }
        }
    }
}