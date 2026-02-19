using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Testcontainers.MongoDb;

namespace Rocket.Tests.Infrastructure
{
    public class ContainerOrchestrator : IContainerOrchestrator
    {
        private INetwork _network;
        private IContainer _apiContainer;
        private MongoDbContainer _dbContainer;

        private const string DbUserName = "mongouser";
        private const string DbPassword = "secretpassword";

        private static readonly string NetworkName = $"integration-network-{Guid.NewGuid()}";

        private const string MongoImageTag = "mongo:latest";
        private const string ApiImageTag = "ghcr.io/gman-au/bottle-rocket-server/bottle-rocket-server-api:smoke";

        public async Task<INetwork> ArrangeDockerNetwork()
        {
            _network =
                new NetworkBuilder()
                    .WithName(NetworkName)
                    .Build();

            await
                _network
                    .CreateAsync();

            return
                _network;
        }

        public async Task TeardownDockerNetwork()
        {
            if (_network != null)
                await
                    _network
                        .DisposeAsync();
        }

        public async Task ArrangeDatabaseResetAsync()
        {
            await
                TeardownDatabaseContainerAsync();

            await
                ArrangeDatabaseContainerAsync();
        }

        public async Task<MongoDbContainer> ArrangeDatabaseContainerAsync()
        {
            _dbContainer =
                new MongoDbBuilder(MongoImageTag)
                    .WithUsername(DbUserName)
                    .WithPassword(DbPassword)
                    .WithName("bottle.rocket.mongodb")
                    .WithHostname("bottle.rocket.mongodb")
                    .WithNetworkAliases("bottle.rocket.mongodb")
                    .WithNetwork(_network)
                    .WithPortBinding(
                        27017,
                        27017
                    )
                    .WithWaitStrategy(
                        Wait
                            .ForUnixContainer()
                            .UntilCommandIsCompleted(
                                "mongosh --eval 'db.runCommand({ ping: 1 }).ok' --quiet || exit 1"
                            )
                    )
                    .Build();

            await
                _dbContainer
                    .StartAsync();

            return
                _dbContainer;
        }

        public async Task TeardownDatabaseContainerAsync()
        {
            if (_dbContainer != null)
                await
                    _dbContainer
                        .DisposeAsync();
        }

        public async Task<IContainer> ArrangeApiContainerAsync()
        {
            _apiContainer =
                new ContainerBuilder(ApiImageTag)
                    .WithName("bottle.rocket.api")
                    .WithHostname("bottle.rocket.api")
                    .WithNetworkAliases("bottle.rocket.api")
                    .WithEnvironment(
                        "ASPNETCORE_ENVIRONMENT",
                        "Development"
                    )
                    .WithEnvironment(
                        "ASPNETCORE_URLS",
                        "http://+:8080"
                    )
                    .WithEnvironment(
                        "MongoDbConfigurationOptions__ConnectionString",
                        $"mongodb://{DbUserName}:{DbPassword}@bottle.rocket.mongodb:27017"
                    )
                    .WithEnvironment(
                        "MongoDbConfigurationOptions__DatabaseName",
                        $"BottleRocket"
                    )
                    .WithEnvironment(
                        "LocalBlobConfigurationOptions__LocalBasePath",
                        "/bottle-rocket"
                    )
                    .WithEnvironment(
                        "LocalBlobConfigurationOptions__LocalSubfolder",
                        "/scans"
                    )
                    .WithPortBinding(
                        3001,
                        8080
                    )
                    .WithVolumeMount("api-scans", "/bottle-rocket/scans")
                    .WithNetwork(_network)
                    .WithWaitStrategy(
                        Wait
                            .ForUnixContainer()
                            .UntilCommandIsCompleted(
                                "curl -f http://localhost:8080/api/health | grep -q 'Status OK' || exit 1"
                            )
                    )
                    .Build();

            await
                _apiContainer
                    .StartAsync();

            return
                _apiContainer;
        }

        public async Task TeardownAppContainersAsync()
        {
            if (_apiContainer != null)
                await
                    _apiContainer
                        .DisposeAsync();
        }
    }
}