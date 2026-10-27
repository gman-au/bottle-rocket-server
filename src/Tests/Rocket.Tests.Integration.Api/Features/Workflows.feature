@DEVICE:API
Feature: Workflows
Description: Testing the Workflows API endpoint and associated functionality.

Background:
    Given the user John has been added as an admin
    Given the user Paula has been added as a non-admin via user John

@UNAUTHORIZED
Scenario: Fetch workflows (unauthorized)
    Given an API request is created against endpoint "/api/workflows/fetch"
    When the request is sent via "POST"
    Then the request should have failed with status code 401    
    
@CREATE-RECORD @INVALID
Scenario: Create new workflow (no required details)
    Given an API request is created against endpoint "/api/workflows/create"
    And the request authorization is set to the user John
    And the request body element "other" has value "nothing"
    When the request is sent via "POST"
    Then the request should have failed with status code 500
    And the response with path "error_code" should have a value of "1002"    
    
@CREATE-RECORD @CLONE
Scenario: Create a workflow and clone it
    Given an API request is created against endpoint "/api/workflows/create"
    And the request authorization is set to the user John
    And the request body element "name" has value "My Workflow 1"
    When the request is sent via "POST"
    Then the request should have succeeded
    # Access as the user that created the scan
    When the response with path "id" is stored with key "workflow-id"
    Given an API request is created against endpoint "/api/workflows/clone"
    And the request authorization is set to the user John
    And the request body element "id" has value stored under "workflow-id"
    And the request body element "name" has value "My Workflow 2"
    When the request is sent via "POST"
    Then the request should have succeeded