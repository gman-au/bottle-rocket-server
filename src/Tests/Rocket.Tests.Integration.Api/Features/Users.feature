@DEVICE:API
Feature: Users

    @UNAUTHORIZED
    Scenario: Fetch users (unauthorized)
        Given an API request is created against endpoint "/api/users/fetch"
        When the request is sent via "POST"
        Then the request should have failed with status code 401

    @FETCH-RECORDS
    Scenario: Fetch users (as admin), first 10 records
        Given an API request is created against endpoint "/api/users/fetch"
        And the request authorization is set to the admin user
        And the request body element "start_index" has value "0"
        And the request body element "record_count" has value "10"
        When the request is sent via "POST"
        Then the request should have succeeded
        And the response with path "users.[0].user_name" should have a value of "admin"
        And the response with path "total_records" should have a value of "1"

    @FETCH-RECORDS
    Scenario: Fetch users (as admin), first 0 records
        Given an API request is created against endpoint "/api/users/fetch"
        And the request authorization is set to the admin user
        And the request body element "start_index" has value "1"
        And the request body element "record_count" has value "0"
        When the request is sent via "POST"
        Then the request should have succeeded
        And the response with path "total_records" should have a value of "1"