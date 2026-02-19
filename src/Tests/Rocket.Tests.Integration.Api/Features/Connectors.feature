@DEVICE:API
Feature: Connectors
Description: Testing the Connectors API endpoint and associated functionality.

    Background:
        Given the user John has been added as an admin
        Given the user Paula has been added as a non-admin via user John

    @UNAUTHORIZED
    Scenario: Fetch connectors (unauthorized)
        Given an API request is created against endpoint "/api/connectors/fetch"
        When the request is sent via "POST"
        Then the request should have failed with status code 401

    @CREATE-RECORD @INVALID
    Scenario: Create new connector (no required details)
        Given an API request is created against endpoint "/api/connectors/create"
        And the request authorization is set to the user John
        And the request body element "connector.$type" has value "ollama_connector"
        When the request is sent via "POST"
        Then the request should have failed with status code 500
        And the response with path "error_code" should have a value of "1002"

    @CREATE-RECORD @INVALID
    Scenario: Create new connector (unrecognized type)
        Given an API request is created against endpoint "/api/connectors/create"
        And the request authorization is set to the user John
        And the request body element "connector.$type" has value "unknown_connector"
        When the request is sent via "POST"
        Then the request should have failed with status code 400

    @CREATE-RECORD
    Scenario: Create new connector (with required details)
        Given an API request is created against endpoint "/api/connectors/create"
        And the request authorization is set to the user John
        And the request body element "connector.$type" has value "ollama_connector"
        And the request body element "connector.endpoint" has value "http://localhost:12345"
        When the request is sent via "POST"
        Then the request should have succeeded

    @CREATE-RECORD @SEGREGATION
    Scenario: Ensure connectors are segregated
        Given an API request is created against endpoint "/api/connectors/create"
        And the request authorization is set to the user John
        And the request body element "connector.$type" has value "ollama_connector"
        And the request body element "connector.endpoint" has value "http://localhost:12345"
        When the request is sent via "POST"
        Then the request should have succeeded
        # Access as the user that created the scan
        Given an API request is created against endpoint "/api/connectors/fetch"
        And the request authorization is set to the user John
        And the request body element "start_index" has value "0"
        And the request body element "record_count" has value "10"
        When the request is sent via "POST"
        Then the request should have succeeded
        And the response with path "total_records" should have a value of "1"
        # Access as the user that didn't create the scan
        Given an API request is created against endpoint "/api/connectors/fetch"
        And the request authorization is set to the user Paula
        And the request body element "start_index" has value "0"
        And the request body element "record_count" has value "10"
        When the request is sent via "POST"
        Then the request should have succeeded
        And the response with path "total_records" should have a value of "0"
        