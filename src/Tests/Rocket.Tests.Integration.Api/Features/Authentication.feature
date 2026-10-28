@DEVICE:API
Feature: Authentication
Description: Testing the login, logout, and authorized API routes.

Background:
    Given the user Admin has a current login session
    And the user John has been added as an admin

@LOGIN
@VALID
Scenario: Login and use the token (correctly) in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    # Access as the user that created the scan
    When the response with path "token" is stored with key "bearer-token"
    And a prefix "Bearer " is added to the value stored in "bearer-token" and stored under "full-api-token"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have succeeded
    
@LOGIN
@INVALID
Scenario: Login and use the token (with wrong auth header name) in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    # Access as the user that created the scan
    When the response with path "token" is stored with key "bearer-token"
    And a prefix "Basic " is added to the value stored in "bearer-token" and stored under "full-api-token"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have failed with status code 401

@LOGIN    
@SHORT-JWT-EXPIRY
Scenario: Login and use an expired token in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    # Wait for the token to expire (should be 20 seconds)
    When 30 seconds have passed
    # Access as the user that created the scan
    When the response with path "token" is stored with key "bearer-token"
    And a prefix "Bearer " is added to the value stored in "bearer-token" and stored under "full-api-token"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have failed with status code 401