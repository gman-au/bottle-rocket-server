@DEVICE:API
Feature: Batches
Description: Testing the Batches Captures API endpoint and associated functionality.

Background:
    Given the user Admin has a current login session
    And the user John has been added as an admin
    And the user John has a current login session
    And the user Paula has been added as a non-admin via user John
    And the user Paula has a current login session

@UNAUTHORIZED
Scenario: Fetch batches (unauthorized)
    Given an API request is created against endpoint "/api/batches/fetch"
    When the request is sent via "POST"
    Then the request should have failed with status code 401

@CREATE-RECORD
Scenario: Send new capture (include attachment data) (batchless)
    Given an API multipart request is created against endpoint "/api/capture/process"
    And the multipart file data is set to base64 string "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAACklEQVR42mMAAQAABQABoIJXOQAAAABJRU5ErkJggg=="
    And the multipart request authorization is set to the user John
    When the multipart request is sent via "POST"
    Then the multipart request should have succeeded

@CREATE-RECORD
Scenario: Open and close a new batch (via the batch ID)
    Given an API request is created against endpoint "/api/batches/open"
    And the request authorization is set to the user John
    When the request is sent via "GET"
    Then the request should have succeeded
    When the response with path "id" is stored with key "batch-id-01"
    Given an API request is created against endpoint "/api/batches/close"
    And the request authorization is set to the user John
    And the request body element "id" has value stored under "batch-id-01"
    When the request is sent via "POST"
    Then the request should have succeeded
    
@INVALID
Scenario: Close a batch (using wrong ID)
    When the response with path "id" is stored with key "batch-id-01"
    Given an API request is created against endpoint "/api/batches/close"
    And the request authorization is set to the user John
    And the request body element "id" has value "111111111111111111111111"
    When the request is sent via "POST"
    Then the request should have failed with status code 500
    And the response with path "error_code" should have a value of "3001"

@INVALID
Scenario: Close a closed batch
    Given an API request is created against endpoint "/api/batches/open"
    And the request authorization is set to the user John
    When the request is sent via "GET"
    Then the request should have succeeded
    When the response with path "id" is stored with key "batch-id-01"
    Given an API request is created against endpoint "/api/batches/close"
    And the request authorization is set to the user John
    And the request body element "id" has value stored under "batch-id-01"
    When the request is sent via "POST"
    Then the request should have succeeded
    Given an API request is created against endpoint "/api/batches/close"
    And the request authorization is set to the user John
    And the request body element "id" has value stored under "batch-id-01"
    When the request is sent via "POST"
    Then the request should have failed with status code 500
    And the response with path "error_code" should have a value of "3006"