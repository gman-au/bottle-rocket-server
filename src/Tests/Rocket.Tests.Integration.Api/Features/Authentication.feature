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
    When the response with path "access_token" is stored with key "bearer-token-1"
    And a prefix "Bearer " is added to the value stored in "bearer-token-1" and stored under "full-api-token-1"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-1"
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
    When the response with path "access_token" is stored with key "bearer-token-2"
    And a prefix "Basic " is added to the value stored in "bearer-token-2" and stored under "full-api-token-2"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-2"
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
    When the response with path "access_token" is stored with key "bearer-token-3"
    And a prefix "Bearer " is added to the value stored in "bearer-token-3" and stored under "full-api-token-3"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-3"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have failed with status code 401
    And the response with path "error_code" should have a value of "2006"
    
@LOGIN
Scenario: Login, logout, and attempt to reuse the revoked token in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    When the response with path "access_token" is stored with key "bearer-token-4"
    And a prefix "Bearer " is added to the value stored in "bearer-token-4" and stored under "full-api-token-4"
    Given an API request is created against endpoint "/api/auth/logout"
    And the request header element "Authorization" has value stored under "full-api-token-4"
    When the request is sent via "POST"
    Then the request should have succeeded
    And the response with path "is_logged_out" should have a value of "true"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-4"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have failed with status code 401
    And the response with path "error_code" should have a value of "2006"