using System.Collections.Generic;
using System.Linq;
using Rocket.Domain.Workflows;

namespace Rocket.Infrastructure.Db.Mongo.Extensions
{
    internal static class BaseWorkflowStepEx
    {
        public static BaseWorkflowStep FindStepById(
            this IEnumerable<BaseWorkflowStep> steps,
            string stepId
        )
        {
            if (steps == null) return null;

            foreach (var step in steps)
            {
                if (step.Id == stepId)
                    return step;

                var found =
                    step
                        .ChildSteps
                        .FindStepById(
                            stepId
                        );

                if (found != null)
                    return found;
            }

            return null;
        }

        public static bool AddChildToParent(
            this IEnumerable<BaseWorkflowStep> steps,
            BaseWorkflowStep workflowStep,
            ref string parentStepId
        )
        {
            if (steps == null) return false;

            foreach (var step in steps)
            {
                if (step.Id == parentStepId)
                {
                    var childList =
                        step
                            .ChildSteps?
                            .ToList() ?? [];

                    childList
                        .Add(workflowStep);

                    step.ChildSteps = childList;

                    return true;
                }

                if (step.ChildSteps.AddChildToParent(workflowStep, ref parentStepId))
                    return true;
            }

            return false;
        }

        public static bool DeleteStepById(this Workflow workflow, string stepId)
        {
            // Check root level first
            var rootSteps =
                workflow
                    .Steps?
                    .ToList();

            if (rootSteps == null) return false;

            for (var i = 0; i < rootSteps.Count; i++)
            {
                if (rootSteps[i].Id != stepId) continue;

                rootSteps
                    .RemoveAt(i);

                workflow.Steps = rootSteps;

                return true;
            }

            // Check nested levels
            foreach (var step in rootSteps)
            {
                if (!DeleteStepFromChildren(step, stepId)) continue;

                workflow.Steps = rootSteps;

                return true;
            }

            return false;
        }

        public static bool DeleteStepFromChildren(
            this BaseWorkflowStep parentStep,
            string stepId
        )
        {
            if (parentStep.ChildSteps == null) return false;

            var childSteps =
                parentStep
                    .ChildSteps
                    .ToList();

            for (var i = 0; i < childSteps.Count; i++)
            {
                if (childSteps[i].Id == stepId)
                {
                    childSteps
                        .RemoveAt(i);

                    parentStep.ChildSteps = childSteps;

                    return true;
                }

                if (!childSteps[i].DeleteStepFromChildren(stepId)) continue;

                parentStep.ChildSteps = childSteps;

                return true;
            }

            return false;
        }
    }
}