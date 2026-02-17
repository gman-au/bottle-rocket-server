@DEVICE:API
Feature: Startup

    @HEALTH-CHECK
    Scenario: Health check endpoint
        Given an API request is created against endpoint "/api/health"
        When the request is sent via "GET"
        Then the request should have succeeded