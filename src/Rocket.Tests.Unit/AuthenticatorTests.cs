using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Rocket.Domain;
using Rocket.Infrastructure;
using Rocket.Interfaces;
using Rocket.Tests.Unit.Extensions;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class AuthenticatorTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public async Task Test_Login_Valid_User()
        {
            _context.ArrangeUserFound();
            _context.ArrangeMatchedPassword();
            await _context.ActPerformLoginAsync();
            _context.AssertUserWasReturned();
            _context.AssertHasherWasCalled();
            _context.AssertLastLoginUpdateWasCalled();
        }

        [Fact]
        public async Task Test_Login_Invalid_Password()
        {
            _context.ArrangeUserFound();
            _context.ArrangeMismatchedPassword();
            await _context.ActPerformLoginAsync();
            _context.AssertUserWasNotReturned();
            _context.AssertHasherWasCalled();
            _context.AssertLastLoginUpdateWasNotCalled();
        }

        [Fact]
        public async Task Test_Login_Not_Found_User()
        {
            _context.ArrangeUserNotFound();
            _context.ArrangeMismatchedPassword();
            await _context.ActPerformLoginAsync();
            _context.AssertUserWasNotReturned();
            _context.AssertHasherWasNotCalled();
            _context.AssertLastLoginUpdateWasNotCalled();
        }

        [Fact]
        public async Task Test_Login_Inactive_User()
        {
            _context.ArrangeUserInactive();
            _context.ArrangeMismatchedPassword();
            await _context.ActPerformLoginAsync();
            _context.AssertUserWasNotReturned();
            _context.AssertHasherWasNotCalled();
            _context.AssertLastLoginUpdateWasNotCalled();
        }

        private class TestContext
        {
            private readonly Authenticator _sut;
            private readonly IUserRepository _userRepository;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IFixture _fixture;
            private User _result;

            public TestContext()
            {
                _fixture =
                    FixtureEx
                        .CreateNSubstituteFixture();

                _userRepository =
                    _fixture
                        .Create<IUserRepository>();

                _passwordHasher =
                    _fixture
                        .Create<IPasswordHasher>();

                _sut =
                    new Authenticator(
                        _fixture.Freeze<ILogger<Authenticator>>(),
                        _passwordHasher,
                        _userRepository
                    );
            }

            public void ArrangeUserFound()
            {
                var user =
                    _fixture
                        .Build<User>()
                        .With(o => o.IsActive, true)
                        .Create();

                _userRepository
                    .GetUserByNameAsync(null, CancellationToken.None)
                    .ReturnsForAnyArgs(Task.FromResult(user));
            }

            public void ArrangeUserInactive()
            {
                var user =
                    _fixture
                        .Build<User>()
                        .With(o => o.IsActive, false)
                        .Create();

                _userRepository
                    .GetUserByNameAsync(null, CancellationToken.None)
                    .ReturnsForAnyArgs(Task.FromResult(user));
            }

            public void ArrangeUserNotFound()
            {
                _userRepository
                    .GetUserByNameAsync(null, CancellationToken.None)
                    .ReturnsForAnyArgs(Task.FromResult<User>(null));
            }

            public void ArrangeMatchedPassword()
            {
                _passwordHasher
                    .VerifyPassword(null, null)
                    .ReturnsForAnyArgs(true);
            }

            public void ArrangeMismatchedPassword()
            {
                _passwordHasher
                    .VerifyPassword(null, null)
                    .ReturnsForAnyArgs(false);
            }

            public async Task ActPerformLoginAsync()
            {
                _result =
                    await
                        _sut
                            .AuthenticateAsync(
                                "user",
                                "password",
                                CancellationToken.None
                            );
            }

            public void AssertUserWasReturned()
            {
                Assert.NotNull(_result);
            }

            public void AssertUserWasNotReturned()
            {
                Assert.Null(_result);
            }

            public void AssertHasherWasCalled()
            {
                _passwordHasher
                    .ReceivedWithAnyArgs(1)
                    .VerifyPassword(null, null);
            }

            public void AssertHasherWasNotCalled()
            {
                _passwordHasher
                    .ReceivedWithAnyArgs(0)
                    .VerifyPassword(null, null);
            }

            public void AssertLastLoginUpdateWasCalled()
            {
                _userRepository
                    .ReceivedWithAnyArgs(1)
                    .UpdateUserFieldAsync<DateTime?>(null, null, null, CancellationToken.None);
            }

            public void AssertLastLoginUpdateWasNotCalled()
            {
                _userRepository
                    .ReceivedWithAnyArgs(0)
                    .UpdateUserFieldAsync<DateTime?>(null, null, null, CancellationToken.None);
            }
        }
    }
}