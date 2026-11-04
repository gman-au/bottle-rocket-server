using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Rocket.Domain.Connectors;
using Rocket.Infrastructure.Encryption;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbStartupEncryptor(
        ILogger<MongoDbStartupEncryptor> logger,
        IMongoDbClient mongoDbClient,
        ISecretProtector secretProtector
    ) : IDatabasePreRunAction
    {
        public async Task RunAsync(
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Running MongoDB startup encryptor...");

            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            var pathsByDiscriminator =
                BsonClassMap
                    .GetRegisteredClassMaps()
                    .Select(cm => cm.ClassType)
                    .Where(t => !t.IsAbstract && typeof(BaseConnector).IsAssignableFrom(t))
                    .Select(BsonClassMap.LookupClassMap)
                    .ToDictionary(
                        cm => cm.Discriminator,
                        cm => EncryptedPaths(
                                cm,
                                [cm.ClassType]
                            )
                            .ToArray()
                    );


            var collection =
                mongoDatabase
                    .GetCollection<BsonDocument>(MongoConstants.ConnectorsCollection);

            var pending =
                await collection
                    .Find(
                        new BsonDocument(
                            "IsEncrypted",
                            new BsonDocument(
                                "$ne",
                                true
                            )
                        )
                    )
                    .ToListAsync(cancellationToken);

            var u = Builders<BsonDocument>.Update;
            var f = Builders<BsonDocument>.Filter;

            var migrated = 0;

            foreach (var doc in pending)
            {
                var updates = new List<UpdateDefinition<BsonDocument>>
                {
                    u.Set(
                        "IsEncrypted",
                        true
                    )
                };

                if (TryGetLeafDiscriminator(
                        doc,
                        out var discriminator
                    ) &&
                    pathsByDiscriminator
                        .TryGetValue(
                            discriminator,
                            out var paths
                        ))
                {
                    foreach (var path in paths)
                    {
                        if (!TryGetString(
                                doc,
                                path,
                                out var value
                            )) continue;

                        updates
                            .Add(
                                u.Set(
                                    path,
                                    secretProtector
                                        .Protect(value)
                                )
                            );
                    }
                }
                else
                {
                    logger
                        .LogWarning(
                            "Connector {Id} has unknown discriminator; skipped",
                            doc["_id"]
                        );
                    continue;
                }

                // Guard on the flag so a concurrent instance can't double-encrypt
                var filter =
                    f
                        .And(
                            f.Eq(
                                "_id",
                                doc["_id"]
                            ),
                            f.Ne(
                                "IsEncrypted",
                                true
                            )
                        );

                var result =
                    await
                        collection
                            .UpdateOneAsync(
                                filter,
                                u.Combine(updates),
                                cancellationToken: cancellationToken
                            );

                if (result.ModifiedCount > 0)
                    migrated++;
            }

            logger
                .LogInformation(
                    "Encryption migration: {Migrated} connector(s) encrypted",
                    migrated
                );
        }

        private static bool TryGetLeafDiscriminator(
            BsonDocument doc,
            out string discriminator
        )
        {
            discriminator = "";

            if (!doc.TryGetValue(
                    "_t",
                    out var t
                )) return false;

            if (t.IsString)
            {
                discriminator = t.AsString;
                return true;
            }

            if (t.IsBsonArray && t.AsBsonArray.Count > 0)
            {
                discriminator = t.AsBsonArray[^1].AsString;
                return true;
            }

            return false;
        }

        private static IEnumerable<string> EncryptedPaths(
            BsonClassMap cm,
            HashSet<Type> visiting
        )
        {
            foreach (var m in cm.AllMemberMaps)
            {
                if (m.GetSerializer() is EncryptedStringSerializer)
                {
                    yield
                        return
                        m
                            .ElementName;
                }
                else if (m.MemberType.IsClass
                         && m.MemberType != typeof(string)
                         && BsonClassMap.IsClassMapRegistered(m.MemberType)
                         && visiting.Add(m.MemberType)) // guards against cycles
                {
                    foreach (var sub in EncryptedPaths(
                                 BsonClassMap.LookupClassMap(m.MemberType),
                                 visiting
                             ))
                        yield
                            return
                            $"{m.ElementName}.{sub}";

                    visiting
                        .Remove(m.MemberType);
                }
            }
        }

        private static bool TryGetString(
            BsonDocument doc,
            string path,
            out string value
        )
        {
            BsonValue current = doc;

            foreach (var part in path.Split('.'))
            {
                if (current is not BsonDocument d || !d.TryGetValue(
                        part,
                        out current
                    ))
                {
                    value = "";
                    return false;
                }
            }

            if (current is BsonString s)
            {
                value = s.Value;
                return true;
            }

            value = "";

            return false;
        }
    }
}