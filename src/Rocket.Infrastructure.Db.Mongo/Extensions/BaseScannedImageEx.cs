using System.Collections.Generic;
using System.Linq;
using Rocket.Domain;
using Rocket.Domain.Batches;
using Rocket.Domain.ScannedImages;

namespace Rocket.Infrastructure.Db.Mongo.Extensions
{
    internal static class BaseScannedImageEx
    {
        public static ScannedImage FindScanById(
            this IEnumerable<ScannedImage> scans,
            string scanId
        )
        {
            if (scans == null) return null;

            foreach (var scan in scans)
            {
                if (scan.Id == scanId)
                    return scan;
            }

            return null;
        }

        public static bool AddScanToBatch(
            this Batch batch,
            ScannedImage scanToAdd
        )
        {
            if (batch?.Scans == null) return false;

            var childList =
                batch
                    .Scans
                    .ToList() ?? [];

            childList
                .Add(scanToAdd);

            batch.Scans = childList;

            return true;
        }

        public static bool UpdateScanById(
            IEnumerable<ScannedImage> scans,
            string scanId,
            ScannedImage updatedScan,
            out IEnumerable<ScannedImage> modifiedScans
        )
        {
            modifiedScans = scans;

            if (scans == null)
                return false;

            var scansList =
                scans
                    .ToList();

            for (var i = 0; i < scansList.Count; i++)
            {
                if (scansList[i].Id == scanId)
                {
                    scansList[i] = updatedScan;
                    modifiedScans = scansList;
                    return true;
                }
            }

            return false;
        }

        public static bool DeleteScanById(
            this Batch batch,
            string scanId
        )
        {
            // Check root level first
            var scans =
                batch
                    .Scans?
                    .ToList();

            if (scans == null) return false;

            for (var i = 0; i < scans.Count; i++)
            {
                if (scans[i].Id != scanId) continue;

                scans
                    .RemoveAt(i);

                batch.Scans = scans;

                return true;
            }

            return false;
        }
    }
}