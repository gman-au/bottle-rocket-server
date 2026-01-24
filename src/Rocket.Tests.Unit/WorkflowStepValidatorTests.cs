using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using NSubstitute;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Workflows;
using Rocket.Infrastructure;
using Rocket.Interfaces;
using Rocket.Tests.Unit.Extensions;
using Xunit;

namespace Rocket.Tests.Unit
{
    public class WorkflowStepValidatorTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public async Task Test_Void_Input()
        {
            _context.ArrangeWorkflowReturned();
            _context.ArrangeParentStepIsNotProvided();
            _context.ArrangeNoParentStepReturned();
            _context.ArrangeChildStepVoidInput();
            await _context.ActValidateWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.DeveloperError);
        }

        [Fact]
        public async Task Test_No_Workflow_Found()
        {
            _context.ArrangeNoWorkflowReturned();
            _context.ArrangeParentStepIsProvided();
            _context.ArrangeParentStepTextDataOutput();
            _context.ArrangeChildStepTextDataInput();
            await _context.ActValidateWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.UnknownOrInaccessibleRecord);
        }

        [Fact]
        public async Task Test_No_Workflow_Parent_Found()
        {
            _context.ArrangeWorkflowReturned();
            _context.ArrangeParentStepIsProvided();
            _context.ArrangeNoParentStepReturned();
            _context.ArrangeChildStepTextDataInput();
            await _context.ActValidateWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.UnknownOrInaccessibleRecord);
        }

        [Fact]
        public async Task Test_Matching_Input_Output_With_Parent()
        {
            _context.ArrangeWorkflowReturned();
            _context.ArrangeParentStepIsProvided();
            _context.ArrangeParentStepTextDataOutput();
            _context.ArrangeChildStepTextDataInput();
            await _context.ActValidateAsync();
            _context.AssertNoException();
        }

        [Fact]
        public async Task Test_Mismatched_Input_Output_With_Parent()
        {
            _context.ArrangeWorkflowReturned();
            _context.ArrangeParentStepIsProvided();
            _context.ArrangeParentStepImageDataOutput();
            _context.ArrangeChildStepTextDataInput();
            await _context.ActValidateWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.ValidationError);
        }

        [Fact]
        public async Task Test_Matched_Input_Output_Without_Parent()
        {
            _context.ArrangeWorkflowReturned();
            _context.ArrangeParentStepIsNotProvided();
            _context.ArrangeChildStepImageDataInput();
            await _context.ActValidateAsync();
            _context.AssertNoException();
        }

        [Fact]
        public async Task Test_Mismatched_Input_Output_Without_Parent()
        {
            _context.ArrangeWorkflowReturned();
            _context.ArrangeParentStepIsNotProvided();
            _context.ArrangeChildStepTextDataInput();
            await _context.ActValidateWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.ValidationError);
        }

        [Fact]
        public async Task Test_Mismatched_Void_Input_Output_With_Parent()
        {
            _context.ArrangeWorkflowReturned();
            _context.ArrangeParentStepIsProvided();
            _context.ArrangeParentStepVoidOutput();
            _context.ArrangeChildStepTextDataInput();
            await _context.ActValidateWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.ValidationError);
        }

        [Fact]
        public async Task Test_Matched_Void_Input_Output_With_Parent()
        {
            _context.ArrangeWorkflowReturned();
            _context.ArrangeParentStepIsProvided();
            _context.ArrangeParentStepVoidOutput();
            _context.ArrangeChildStepVoidInput();
            await _context.ActValidateWithExceptionAsync();
            _context.AssertExceptionCode(ApiStatusCodeEnum.ValidationError);
        }

        private class TestContext
        {
            private readonly WorkflowStepValidator _sut;
            private readonly IFixture _fixture;
            private readonly IWorkflowStepRepository _workflowStepRepository;
            private string _parentStepId;
            private int[] _childInputTypes;
            private RocketException _exception;

            public TestContext()
            {
                _fixture =
                    FixtureEx
                        .CreateNSubstituteFixture();

                _workflowStepRepository =
                    _fixture
                        .Create<IWorkflowStepRepository>();

                _sut =
                    new WorkflowStepValidator(_workflowStepRepository);
            }

            public void ArrangeWorkflowReturned()
            {
                _workflowStepRepository
                    .GetWorkflowByIdAsync(
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(
                        _fixture.Build<Workflow>()
                            .Create()
                    );
            }

            public void ArrangeNoWorkflowReturned()
            {
                _workflowStepRepository
                    .GetWorkflowByIdAsync(
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(Task.FromResult<Workflow>(null));
            }

            public void ArrangeChildStepTextDataInput() => ArrangeChildStepWithInput((int)WorkflowFormatTypeEnum.RawTextData);
            public void ArrangeChildStepImageDataInput() => ArrangeChildStepWithInput((int)WorkflowFormatTypeEnum.ImageData);
            public void ArrangeChildStepVoidInput() => ArrangeChildStepWithInput((int)WorkflowFormatTypeEnum.Void);

            public void ArrangeParentStepTextDataOutput() => ArrangeParentStepWithOutput((int)WorkflowFormatTypeEnum.RawTextData);
            public void ArrangeParentStepImageDataOutput() => ArrangeParentStepWithOutput((int)WorkflowFormatTypeEnum.ImageData);
            public void ArrangeParentStepVoidOutput() => ArrangeParentStepWithOutput((int)WorkflowFormatTypeEnum.Void);

            public void ArrangeParentStepIsProvided() => _parentStepId = "12345";
            public void ArrangeParentStepIsNotProvided() => _parentStepId = null;

            private void ArrangeChildStepWithInput(int inputType) => _childInputTypes = [inputType];

            public void ArrangeNoParentStepReturned()
            {
                _workflowStepRepository
                    .GetWorkflowStepByIdAsync(
                        null,
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(
                        Task.FromResult<BaseWorkflowStep>(null)
                    );
            }

            private void ArrangeParentStepWithOutput(int outputType)
            {
                _workflowStepRepository
                    .GetWorkflowStepByIdAsync(
                        null,
                        null,
                        null,
                        CancellationToken.None
                    )
                    .ReturnsForAnyArgs(
                        _fixture.Build<DummyStep>()
                            .With(o => o.OutputType, outputType)
                            .Create()
                    );
            }

            public async Task ActValidateAsync()
            {
                await
                    _sut
                        .ValidateAsync(
                            "12345",
                            _parentStepId,
                            "12345",
                            _childInputTypes,
                            CancellationToken.None
                        );
            }

            public async Task ActValidateWithExceptionAsync() =>
                _exception =
                    await
                        Assert
                            .ThrowsAsync<RocketException>(ActValidateAsync);

            public void AssertNoException()
            {
                Assert.Null(_exception);
            }

            public void AssertExceptionCode(ApiStatusCodeEnum expected)
            {
                Assert.NotNull(_exception);
                Assert.Equal(
                    (int)expected,
                    _exception.ApiStatusCode
                );
            }

            private record DummyStep : BaseWorkflowStep
            {
                public override int[] InputTypes { get; set; }
                public override int OutputType { get; set; }
                public override string StepName { get; set; }
                public override string RequiresConnectorCode { get; set; }
            }
        }
    }
}