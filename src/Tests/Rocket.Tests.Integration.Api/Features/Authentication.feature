@DEVICE:API
Feature: Authentication
Description: Testing the login, logout, and authorized API routes.

Background:
    Given the user Admin has a current login session
    And the user John has been added as an admin

@LOGIN
@VALID
Scenario: Login and use the access token (correctly) in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    # Access as the user that created the scan
    When the response with path "access_token" is stored with key "bearer-token-01"
    And a prefix "Bearer " is added to the value stored in "bearer-token-01" and stored under "full-api-token-01"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-01"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have succeeded

@LOGIN
@INVALID
Scenario: Login and use the access token (with wrong auth header name) in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    # Access as the user that created the scan
    When the response with path "access_token" is stored with key "bearer-token-02"
    And a prefix "Basic " is added to the value stored in "bearer-token-02" and stored under "full-api-token-02"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-02"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have failed with status code 401

@LOGIN
@SHORT-JWT-EXPIRY
Scenario: Login and use an expired access token in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    # Wait for the token to expire (should be 20 seconds)
    When 30 seconds have passed
    # Access as the user that created the scan
    When the response with path "access_token" is stored with key "bearer-token-03"
    And a prefix "Bearer " is added to the value stored in "bearer-token-03" and stored under "full-api-token-03"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-03"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have failed with status code 401
    And the response with path "error_code" should have a value of "2006"

@LOGIN
@LOGOUT
Scenario: Login, logout, and attempt to reuse the revoked access token in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    When the response with path "access_token" is stored with key "bearer-token-04"
    And a prefix "Bearer " is added to the value stored in "bearer-token-04" and stored under "full-api-token-04"
    Given an API request is created against endpoint "/api/auth/logout"
    And the request header element "Authorization" has value stored under "full-api-token-04"
    When the request is sent via "POST"
    Then the request should have succeeded
    And the response with path "is_logged_out" should have a value of "true"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-04"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have failed with status code 401
    And the response with path "error_code" should have a value of "2006"

@LOGIN
@LOGOUT
Scenario: Login with two tokens, revoke the second, and attempt to reuse the first revoked token in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    When the response with path "access_token" is stored with key "bearer-token-05"
    And a prefix "Bearer " is added to the value stored in "bearer-token-05" and stored under "full-api-token-05"
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    When the response with path "access_token" is stored with key "bearer-token-06"
    When a prefix "Bearer " is added to the value stored in "bearer-token-06" and stored under "full-api-token-06"
    Given an API request is created against endpoint "/api/auth/logout"
    And the request header element "Authorization" has value stored under "full-api-token-06"
    When the request is sent via "POST"
    Then the request should have succeeded
    And the response with path "is_logged_out" should have a value of "true"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-05"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have succeeded
    
@LOGIN
@LOGOUT
Scenario: Login with two tokens, revoke all, and attempt to reuse the first revoked token in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    When the response with path "access_token" is stored with key "bearer-token-07"
    And a prefix "Bearer " is added to the value stored in "bearer-token-07" and stored under "full-api-token-07"
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    Then the request should have succeeded
    When the response with path "access_token" is stored with key "bearer-token-08"
    When a prefix "Bearer " is added to the value stored in "bearer-token-08" and stored under "full-api-token-08"
    Given an API request is created against endpoint "/api/auth/logout"
    And the request body element "logout_all" has a Boolean value of True
    And the request header element "Authorization" has value stored under "full-api-token-08"
    When the request is sent via "POST"
    Then the request should have succeeded
    And the response with path "is_logged_out" should have a value of "true"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-07"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have failed with status code 401
    And the response with path "error_code" should have a value of "2006"
    
@REFRESH
@VALID
Scenario: Login and use the refresh token to retrieve a new access token, then use that in a request
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    When the response with path "refresh_token" is stored with key "refresh-token-09"
    # Refresh the token
    Given an API request is created against endpoint "/api/auth/refresh"
    And the request body element "refresh_token" has value stored under "refresh-token-09"
    When the request is sent via "POST"
    Then the request should have succeeded
    When the response with path "access_token" is stored with key "bearer-token-10"
    # Access as the user that created the scan
    And a prefix "Bearer " is added to the value stored in "bearer-token-10" and stored under "full-api-token-10"
    Given an API request is created against endpoint "/api/users/fetch"
    And the request header element "Authorization" has value stored under "full-api-token-10"
    And the request body element "start_index" has value "1"
    And the request body element "record_count" has value "0"
    When the request is sent via "POST"
    Then the request should have succeeded    
    
@REFRESH
@INVALID
Scenario: Login and use the refresh token to retrieve a new access token, then try to reuse the old refresh token
    Given an API request is created against endpoint "/api/auth/login"
    And the request body element "user_name" has value "john@test.com"
    And the request body element "password" has value "P@ssword!23"
    When the request is sent via "POST"
    Then the request should have succeeded
    When the response with path "refresh_token" is stored with key "refresh-token-11"
    # Refresh the token
    Given an API request is created against endpoint "/api/auth/refresh"
    And the request body element "refresh_token" has value stored under "refresh-token-11"
    When the request is sent via "POST"
    Then the request should have succeeded
    # Refresh the token again
    Given an API request is created against endpoint "/api/auth/refresh"
    And the request body element "refresh_token" has value stored under "refresh-token-11"
    When the request is sent via "POST"
    Then the request should have failed with status code 401