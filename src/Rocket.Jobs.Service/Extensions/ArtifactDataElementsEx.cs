using System;
using System.Collections.Generic;
using Rocket.Domain.Jobs;

namespace Rocket.Jobs.Service.Extensions
{
    internal static class ArtifactDataElementsEx
    {
        public static ArtifactDataElement[] EnumerateUniquely(
            this ArtifactDataElement[] elements
        )
        {
            var result = new string[elements.Length];
            
            var used = 
                new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            var duplicateIndexes = new List<int>();
            
            for (var i = 0; i < elements.Length; i++)
            {
                if (used.Add(elements[i].FileName))
                    result[i] = elements[i].FileName;
                else
                    duplicateIndexes.Add(i);
            }
            
            foreach (var i in duplicateIndexes)
            {
                var stem = elements[i].FileName;
 
                string candidate;
                var n = 1;
                do
                {
                    candidate = $"{stem} ({n++})";
                }
                while (!used.Add(candidate));
 
                result[i] = candidate;
            }

            for (var i = 0; i < elements.Length; i++)
            {
                elements[i].FileName = result[i];
            }

            return elements;
        }
    }
}