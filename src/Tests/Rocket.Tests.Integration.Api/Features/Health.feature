@DEVICE:API
Feature: Health
Description: Testing basic API connectivity endpoints.

    @HEALTH-CHECK
    Scenario: Health check endpoint (get)
        Given an API request is created against endpoint "/api/health"
        When the request is sent via "GET"
        Then the request should have succeeded

    @HEALTH-CHECK @INVALID
    Scenario: Health check endpoint (post)
        Given an API request is created against endpoint "/api/health"
        When the request is sent via "POST"
        Then the request should have failed with status code 405

    @HEALTH-CHECK
    Scenario: Startup phase (get) before admin creation
        Given an API request is created against endpoint "/api/startup/phase"
        When the request is sent via "GET"
        Then the request should have succeeded
        And the response with path "phase" should have a value of "1"

    @HEALTH-CHECK
    Scenario: Startup phase (get) after admin creation
        Given the test user has been added as an admin
        And an API request is created against endpoint "/api/startup/phase"
        When the request is sent via "GET"
        Then the request should have succeeded
        And the response with path "phase" should have a value of "2"