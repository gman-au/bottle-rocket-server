# Developing a Bottle Rocket integration

## API Steps / Checklist

### 1. Create project folders
* Create the project folder under the `/Integrations` sub folder in the solution.
* Add the four main projects to the sub folder:
    * `Rocket.Abc.Domain` - this is the root domain library where Bottle Rocket domain objects are defined as derived classes and extended
    * `Rocket.Abc.Contracts` - this is the contracts library where Bottle Rocket API contracts are defined as derived classes as extended. If you have particular API requests that need to be added, they also go here (e.g. `FinalizeDropboxConnectorRequest`)
    * `Rocket.Abc.Infrastructure` - define all of the integration-specific interfaces and implementations here - this project should contain everything needed to complete integration actions e.g. uploading a note, emailing a file, etc.
    * `Rocket.Abc.Injection` - this is the injection library that should define a single extension method to inject all of the ABC integration into the main Bottle Rocket system.

### 2. Create connectors
* Derive new connectors from `BaseConnector`.
* Set the `ConnectorType`, `ConnectorName`, and `ConnectorCode` override values - define a `DomainConstants` static library if required.
* Define any custom properties for the connector in this class.
* Define the logic to indicate the `ConnectorStatusEnum` via the `DetermineStatus()` logic.

### 3. Create workflow steps
* Derive new workflow steps from `BaseWorkflowStep`.
* Set the `InputTypes`, `OutputType`, `StepName`, and `RequiresConnectorCode` override values - define a `DomainConstants` static library if required.
* Define any custom properties for the workflow step in this class.

### 4. Create execution steps
* Derive new workflow steps from `BaseExecutionStep`.
* You only need to copy custom properties defined in the associated `BaseWorkflowStep` in this class.

### 5. Create API contracts
* You don't need to create bespoke API "Create Connection Request" contracts, you can use the main `CreateConnectorRequest` class and derive `AbcStepSpecifics` of type `ConnectorSummary`.
* Ensure that custom properties are carried through in this class and have the correct `JsonPropertyName` casing.
* Derive new workflow step specific types from `WorkflowStepSummary`, copying custom properties as needed.
* Clone them into classes derived from `ExecutionStepSummary`, copying custom properties as needed.

### 6. Define mapping infrastructure
* Define an `AbcBsonMapper` implementation of `IBsonMapper` - this implementation should register the BSON **domain** class maps to the appropriate types (i.e. connector, workflow step, execution step).
* Create an `AbcConnectorMapper` implementation, derived from `ConnectorModelMapperBase<AbcConnector, AbcConnectorSpecifics>` and implement the required interface methods.
    * Ensure that the `For` and `From` methods set any custom connector properties (the `base()` methods handle the common properties).
    * Use the `PreUpdateAsync()` override to perform validation of the data and throw exceptions if invalid.
* Create an `AbcWorkflowStepMapper` implementation, derived from `WorkflowStepModelMapperBase<AbcWorkflowStep, AbcWorkflowStepSpecifics>` and implement the required interface methods.
    * Ensure that the `For` and `From` methods set any custom connector properties (the `base()` methods handle the common properties).
* Create an `AbcExecutionStepMapper` implementation, derived from `ExecutionStepModelMapperBase<AbcExecutionStep, AbcExecutionStepSpecifics>` and implement the required interface methods.
    * Ensure that the `For` and `From` methods set any custom connector properties (the `base()` methods handle the common properties).
* Create an `AbcStepCloner` implementation, derived from `StepModelClonerBase<AbcWorkflowStep, AbcExecutionStep>` and implement the required interface methods.
    * Ensure that the `Clone` method sets any custom connector properties (the `base()` method handles the common properties).

### 7. Define hook and complete implementation
* Create an `AbcHook` implementation of `IIntegrationHook`.
    * Override `IsApplicable(BaseExecutionStep step)` - it is simply a type check of `step is AbcExecutionStep`.
    * Use the context methods and artifact outputs to complete the ABC integration by performing the necessary 3rd party integration functions inside this hook, abstracting out larger pieces of code into separate interfaces as needed. 
        * These interfaces should reside inside this infrastructure library and do not belong in the main Bottle Rocket code.
    * The function should return a new `ExecutionStepArtifact` of the applicable type where it is expected (e.g. if a step converts X to Y, the artifact should return of type Y).

### 8. Create injection module
* Create a single `ServiceCollectionExtension` module in the injection project, loading all of the `Abc` / vendor-specific implemented interfaces in a single function.

### 9. Add call to injector
* In the API `Program.cs`, you should now only need to add a single line to the injection module.
```
services
    .AddAbcIntegration();
```

### 10. Add entries into the JSON type discriminators
* In order for the API contracts to distinguish the derived types as supplied (Connector, WorkflowStep, ExecutionStep), entries for each need to be added to the `ConnectorTypeDiscriminatorMap`, `ExecutionStepTypeDiscriminatorMap`, and `WorkflowStepTypeDiscriminatorMap` classes in the `Rocket.Infrastructure.Json` project.
    * Add the related API class (e.g. `AbcConnectorSpecifics`) and a unique string in that list to ensure that the type discriminator successfully parses the derived class type.

## Web Steps / Checklist

### 1. Add a vendor-specific connector page
* Add a razor page under `/Components/Pages/Connectors/Vendors/AddAbcConnector.razor`.
    * The page should be instructive as to any steps needed to gather information that is supplied to the page to save particulars of the connection.
    * In simple cases, the submit method should be able to make an `ApiRequestManager.CreateConnectorAsync` typed call of your `AbcConnectorSpecifics` class, as opposed to defining your own API contract + controller method.
* Add your new connector as a card under `/Components/Pages/Connectors/AddConnector.razor`.
    * Add a logo image (preferably white backgrounded instead of transparent).

### 2. Add a vendor-specific workflow step page
* Add a razor page under `/Components/Pages/Workflows/Vendors/AbcStepDetails.razor`.
    * The page should have the following routes defined:
        * `@page "/MyWorkflow/Abc/{workflowId}/AddStep"`
        * `@page "/MyWorkflow/Abc/{workflowId}/Steps/{parentStepId}/AddStep"`
        * `@page "/MyWorkflow/Abc/{workflowId}/Steps/{stepId}/UpdateStep"`
    * For convenience, use the `<UpdateWorkflowStepWrapper>` component to wrap the majority of form
    components (connection filter, layout, etc), and add any step-specific form inputs inside the tag.
* Add your new workflow step as a card under `/Components/Pages/Workflows/AddWorkflowStep.razor`.
    * Add a logo image (preferably white backgrounded instead of transparent).

### 3. Add handler to WriteNested() Mermaid converter
* In the `WorkflowMermaidConverter.cs` file, add a provision for your new `AbcStepSpecifics` class to define the "update step" route for steps of that type; typically this will look like:
```
if (step is AbcWorkflowStepSpecifics)
{
    route = $"/MyWorkflow/Abc/{workflowId}/Steps/{step.Id}/UpdateStep";
}
```

> [!TIP]
> 
> To test your integrations, you can use the diagnostic "Hello World" workflow steps to generate a dummy text output artifact, instead of plugging in an OCR step and waiting for it to finish each time.