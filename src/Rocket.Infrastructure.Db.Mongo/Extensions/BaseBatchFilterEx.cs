using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Rocket.Domain.Batches;
using Rocket.Domain.ScannedImages;

namespace Rocket.Infrastructure.Db.Mongo.Extensions
{
    internal static class BaseBatchFilterEx
    {
        private static readonly string[] BatchSearchTermFields =
        {
            "Transcription.Text",
            "Transcription.Notes",
            "Transcription.Items.Project",
            "Transcription.Items.Task",
            "Transcription.Monday",
            "Transcription.Tuesday",
            "Transcription.Wednesday",
            "Transcription.Thursday",
            "Transcription.Friday",
            "Transcription.Saturday",
            "Transcription.Sunday",
        };

        public static FilterDefinition<Batch> AddBatchSearchTermFilter(
            this FilterDefinitionBuilder<Batch> batchFilterBuilder,
            string searchTerm
        )
        {
            var regex =
                new BsonRegularExpression(
                    Regex.Escape(searchTerm.Trim()),
                    "i"
                );

            var scanBuilder =
                Builders<ScannedImage>
                    .Filter;

            var scanFilters =
                new List<FilterDefinition<ScannedImage>>
                {
                    scanBuilder
                        .Regex(
                            s =>
                                s.Description,
                            regex
                        )
                };

            foreach (var batchSearchTerm in BatchSearchTermFields)
            {
                scanFilters
                    .Add(
                        scanBuilder
                            .Regex(
                                batchSearchTerm,
                                regex
                            )
                    );
            }

            var descriptionFilter =
                batchFilterBuilder
                    .Regex(
                        o => o.Description,
                        regex
                    );

            return
                batchFilterBuilder
                    .Or(
                        batchFilterBuilder
                            .ElemMatch(
                                b =>
                                    b.Scans,
                                scanBuilder
                                    .Or(
                                        scanFilters
                                    )
                            ),
                        descriptionFilter
                    );
        }
    }
}