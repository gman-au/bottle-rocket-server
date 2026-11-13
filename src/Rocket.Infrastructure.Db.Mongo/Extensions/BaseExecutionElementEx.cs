using System.Collections.Generic;
using System.Linq;
using Rocket.Domain.Executions;

namespace Rocket.Infrastructure.Db.Mongo.Extensions
{
    public static class BaseExecutionElementEx
    {
        public static ExecutionElement FindElementById(
            this IEnumerable<ExecutionElement> elements,
            string elementId
        )
        {
            if (elements == null) return null;

            foreach (var element in elements)
            {
                if (element.Id == elementId)
                    return element;
            }

            return null;
        }        
        
        public static ExecutionElement FindElementByScanId(
            this IEnumerable<ExecutionElement> elements,
            string scanId
        )
        {
            if (elements == null) return null;

            foreach (var element in elements)
            {
                if (element.ScanId == scanId)
                    return element;
            }

            return null;
        }

        public static bool UpdateElementById(
            IEnumerable<ExecutionElement> elements,
            string elementId,
            ExecutionElement updatedElement,
            out IEnumerable<ExecutionElement> modifiedElements
        )
        {
            modifiedElements = elements;

            if (elements == null)
                return false;

            var elementsList =
                elements
                    .ToList();

            for (var i = 0; i < elementsList.Count; i++)
            {
                if (elementsList[i].Id == elementId)
                {
                    elementsList[i] = updatedElement;
                    modifiedElements = elementsList;
                    return true;
                }
            }

            return false;
        }

        public static bool AppendLogMessageForScanId(
            IEnumerable<ExecutionElement> elements,
            string scanId,
            string logMessage,
            out IEnumerable<ExecutionElement> modifiedElements
        )
        {
            modifiedElements = elements;

            if (elements == null)
                return false;

            var elementsList =
                elements
                    .ToList();

            for (var i = 0; i < elementsList.Count; i++)
            {
                if (elementsList[i].ScanId == scanId)
                {
                    var logMessages = (elementsList[i].LogMessages ?? []).ToList();
                    logMessages.Add(logMessage);
                    elementsList[i].LogMessages = logMessages.ToArray();
                    modifiedElements = elementsList;
                    return true;
                }
            }

            return false;
        }
    }
}