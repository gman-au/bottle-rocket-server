using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class WorkflowStepValidator(
        IWorkflowStepRepository workflowStepRepository
    ) : IWorkflowStepValidator
    {
        public async Task ValidateAsync(
            string workflowId,
            string parentStepId,
            string userId,
            int[ ]childInputTypes,
            CancellationToken cancellationToken
        )
        {
            var workflow =
                await
                    workflowStepRepository
                        .GetWorkflowByIdAsync(
                            workflowId,
                            userId,
                            cancellationToken
                        );

            if (workflow == null)
                throw new RocketException(
                    $"Workflow record with ID {workflowId} not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var parentOutputType = (int)WorkflowFormatTypeEnum.ImageData;

            if (!string.IsNullOrEmpty(parentStepId))
            {
                var parentStep =
                    await
                        workflowStepRepository
                            .GetWorkflowStepByIdAsync(
                                parentStepId,
                                workflowId,
                                userId,
                                cancellationToken
                            );

                if (parentStep == null)
                    throw new RocketException(
                        $"Workflow step record with ID {parentStepId} not found",
                        ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                    );

                parentOutputType = parentStep.OutputType;

                if (parentOutputType == (int)WorkflowFormatTypeEnum.Void)
                    throw new RocketException(
                        "The workflow step cannot be added as the parent step produces no output.",
                        ApiStatusCodeEnum.ValidationError
                    );
            }

            if (childInputTypes.Any(o => o == (int)WorkflowFormatTypeEnum.Void))
                throw new RocketException(
                    "This workflow step has been incorrectly configured." +
                    "This is a developer error and should be reported.",
                    ApiStatusCodeEnum.DeveloperError
                );

            if (childInputTypes.All(o => o != parentOutputType))
                throw new RocketException(
                    "The workflow step cannot be added as the expected input is not produced by the parent step.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}