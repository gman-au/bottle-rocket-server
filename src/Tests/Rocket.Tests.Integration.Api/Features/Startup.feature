@DEVICE:API
Feature: Startup phase
Description: Testing the API endpoints during system startup and associated functionality.

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

    @CREATE-RECORD @UNAUTHORIZED
    Scenario: Add a new (administrator) user during system startup, then try to fetch users
        Given an API request is created against endpoint "/api/users/create"
        And the request authorization is set to the admin user
        And the request body element "user_name" has value "user@test.com"
        And the request body element "password" has value "password123"
        And the request body element "is_the_new_admin" has a boolean value of true
        When the request is sent via "POST"
        Then the request should have succeeded
        Given an API request is created against endpoint "/api/users/fetch"
        And the request authorization is set to the admin user
        And the request body element "start_index" has value "1"
        And the request body element "record_count" has value "0"
        When the request is sent via "POST"
        Then the request should have failed with status code 401

    @CREATE-RECORD
    Scenario: Add a new (non-administrator) user during system startup
        Given an API request is created against endpoint "/api/users/create"
        And the request authorization is set to the admin user
        And the request body element "user_name" has value "user@test.com"
        And the request body element "password" has value "password123"
        And the request body element "is_the_new_admin" has a boolean value of false
        When the request is sent via "POST"
        Then the request should have failed with status code 500
        And the response with path "error_code" should have a value of "2008"