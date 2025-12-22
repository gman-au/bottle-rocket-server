using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Api.Host.Injection;
using Rocket.Infrastructure.Db.Mongo;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class DependencyInjectionTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public void Test_Missing_Configuration()
        {
            _context.ArrangeServiceLoading();
            _context.ActInjectServices();
            Assert.Throws<ConfigurationErrorsException>(() => _context.AssertServicesCanBeResolved());
        }

        [Fact]
        public void Test_Invalid_Configuration()
        {
            _context.ArrangeInvalidConfiguration();
            _context.ArrangeServiceLoading();
            _context.ActInjectServices();
            Assert.Throws<ConfigurationErrorsException>(() => _context.AssertServicesCanBeResolved());
        }

        [Fact]
        public void Test_Valid_Service_Loading()
        {
            _context.ArrangeValidConfiguration();
            _context.ArrangeServiceLoading();
            _context.ActInjectServices();
            _context.AssertServicesCanBeResolved();
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
                    .AddBottleRocketApiServices()
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