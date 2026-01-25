using System.Collections.Generic;
using System.Linq;
using Rocket.Domain.Executions;

namespace Rocket.Infrastructure.Db.Mongo.Extensions
{
    public static class BaseExecutionStepEx
    {
        public static bool UpdateStepById(
            IEnumerable<BaseExecutionStep> steps,
            string stepId,
            BaseExecutionStep updatedStep,
            out IEnumerable<BaseExecutionStep> modifiedSteps
        )
        {
            modifiedSteps = steps;

            if (steps == null)
                return false;

            var stepsList =
                steps
                    .ToList();

            for (var i = 0; i < stepsList.Count; i++)
            {
                if (stepsList[i].Id == stepId)
                {
                    // Preserve ChildSteps from original
                    updatedStep.ChildSteps =
                        stepsList[i]
                            .ChildSteps;

                    stepsList[i] = updatedStep;
                    modifiedSteps = stepsList;
                    return true;
                }

                if (UpdateStepById(
                        stepsList[i].ChildSteps,
                        stepId,
                        updatedStep,
                        out var modifiedChildren
                    ))
                {
                    stepsList[i].ChildSteps = modifiedChildren;
                    modifiedSteps = stepsList;
                    return true;
                }
            }

            return false;
        }

        public static bool AppendLogMessageForStepId(
            IEnumerable<BaseExecutionStep> steps,
            string stepId,
            string logMessage,
            out IEnumerable<BaseExecutionStep> modifiedSteps
        )
        {
            modifiedSteps = steps;

            if (steps == null)
                return false;

            var stepsList =
                steps
                    .ToList();

            for (var i = 0; i < stepsList.Count; i++)
            {
                if (stepsList[i].Id == stepId)
                {
                    var logMessages = (stepsList[i].LogMessages ?? []).ToList();
                    logMessages.Add(logMessage);
                    stepsList[i].LogMessages = logMessages.ToArray();
                    modifiedSteps = stepsList;
                    return true;
                }

                if (AppendLogMessageForStepId(
                        stepsList[i].ChildSteps,
                        stepId,
                        logMessage,
                        out var modifiedChildren
                    ))
                {
                    stepsList[i].ChildSteps = modifiedChildren;
                    modifiedSteps = stepsList;
                    return true;
                }
            }

            return false;
        }
    }
}