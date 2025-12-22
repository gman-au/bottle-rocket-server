using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Api.Host.Injection;
using Rocket.Infrastructure.MongoDb;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class DependencyInjectionTests
    {
        private readonly TestContext _testContext = new();

        [Fact]
        public void Test_Missing_Configuration()
        {
            _testContext.ArrangeServiceLoading();
            _testContext.ActInjectServices();
            Assert.Throws<ConfigurationErrorsException>(() => _testContext.AssertServicesCanBeResolved());
        }

        [Fact]
        public void Test_Invalid_Configuration()
        {
            _testContext.ArrangeInvalidConfiguration();
            _testContext.ArrangeServiceLoading();
            _testContext.ActInjectServices();
            Assert.Throws<ConfigurationErrorsException>(() => _testContext.AssertServicesCanBeResolved());
        }

        [Fact]
        public void Test_Valid_Service_Loading()
        {
            _testContext.ArrangeValidConfiguration();
            _testContext.ArrangeServiceLoading();
            _testContext.ActInjectServices();
            _testContext.AssertServicesCanBeResolved();
        }

        private class TestContext
        {
            private readonly IServiceCollection _services = new ServiceCollection();
            private IConfigurationRoot _configuration = new ConfigurationBuilder().Build();
            private IServiceProvider _serviceProvider;

            public void ArrangeInvalidConfiguration()
            {
                var inMemoryCollection = new Dictionary<string, string>
                {
                    ["MongoDbConfigurationOptions:ConnectionString"] = "ABCDEFGHIJKLM"
                };

                _configuration =
                    new ConfigurationBuilder()
                        .AddConfiguration(_configuration)
                        .AddInMemoryCollection(inMemoryCollection)
                        .Build();
            }

            public void ArrangeValidConfiguration()
            {
                var inMemoryCollection = new Dictionary<string, string>
                {
                    ["MongoDbConfigurationOptions:ConnectionString"] = "mongodb://127.0.0.1:27017"
                };

                _configuration =
                    new ConfigurationBuilder()
                        .AddConfiguration(_configuration)
                        .AddInMemoryCollection(inMemoryCollection)
                        .Build();
            }

            public void ArrangeServiceLoading()
            {
                _services
                    .AddLogging(o => o.AddConsole())
                    .AddMongoDbServices(_configuration);
            }

            public void ActInjectServices()
            {
                _serviceProvider =
                    _services
                        .BuildServiceProvider();
            }

            public void AssertServicesCanBeResolved()
            {
                object resolvedService =
                    _serviceProvider
                        .GetService<IMongoDbClient>();

                Assert
                    .NotNull(resolvedService);
            }
        }
    }
}